import os
import json
from typing import List, Optional, Dict, Any
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
from llama_cpp import Llama
import uvicorn

# ----------------------
# Config from env
# ----------------------
MODEL_PATH   = os.environ.get("MODEL_PATH", "models/mistral-7b-instruct.Q4_K_M.gguf")
CTX_SIZE     = int(os.environ.get("CTX_SIZE", "4096"))
N_GPU_LAYERS = int(os.environ.get("N_GPU_LAYERS", "0"))      # 0 = CPU only
THREADS      = int(os.environ.get("THREADS", str(os.cpu_count() or 4)))
HOST         = os.environ.get("HOST", "0.0.0.0")
PORT         = int(os.environ.get("PORT", "1234"))

if not os.path.isfile(MODEL_PATH):
    raise RuntimeError(f"MODEL_PATH does not exist: {MODEL_PATH}")

# ----------------------
# Load model
# ----------------------
def try_load_model():
    print("="*70)
    print("🧠  Loading local Mistral GGUF model...")
    print(f"MODEL_PATH   : {MODEL_PATH}")
    print(f"CTX_SIZE     : {CTX_SIZE}")
    print(f"THREADS      : {THREADS}")
    print(f"N_GPU_LAYERS : {N_GPU_LAYERS}")
    print("="*70)

    try:
        llm_obj = Llama(
            model_path=MODEL_PATH,
            n_ctx=CTX_SIZE,
            n_threads=THREADS,
            n_gpu_layers=N_GPU_LAYERS,
            use_mmap=False,
            use_mlock=False,
            verbose=True
        )
        print("✅ Model loaded successfully!\n")
        return llm_obj

    except Exception as e:
        import traceback, sys
        print("❌ Failed to load model.")
        print("Python Exception Type :", type(e).__name__)
        print("Python Exception Value:", e)
        print("Full traceback:")
        traceback.print_exc(file=sys.stdout)
        print("\nPossible causes:")
        print("  • MODEL_PATH is incorrect or file not found")
        print("  • File corrupted (check .gguf file integrity)")
        print("  • You have a non-compatible quantization (wrong model type)")
        print("  • You compiled llama-cpp-python without GPU/CPU backend support")
        print("  • Your environment lacks OpenBLAS / AVX2 / CUDA runtime libs")
        print("="*70)
        raise

llm = try_load_model()

# ----------------------
# Request schemas
# ----------------------
class ChatMessage(BaseModel):
    role: str
    content: str

class ChatCompletionsRequest(BaseModel):
    model: Optional[str] = "mistral-gguf"
    messages: List[ChatMessage]
    temperature: Optional[float] = 0.2
    max_tokens: Optional[int] = 512
    top_p: Optional[float] = 0.95
    stop: Optional[List[str]] = None

# ----------------------
# FastAPI app
# ----------------------
app = FastAPI(
    title="Local GGUF Chat API - Plain Text",
    version="1.0.0"
)

@app.post("/v1/chat/completions")
def chat_completions(req: ChatCompletionsRequest):
    """
    Returns plain text response only - no JSON wrapper
    """
    if not req.messages:
        raise HTTPException(status_code=400, detail="messages is required")

    # Convert to llama.cpp format
    chat = []
    for m in req.messages:
        chat.append({"role": m.role, "content": m.content})

    try:
        # Generate response
        out = llm.create_chat_completion(
            messages=chat,
            temperature=req.temperature or 0.2,
            max_tokens=req.max_tokens or 512,
            top_p=req.top_p or 0.95,
            stop=req.stop or None,
        )
    except Exception as e:
        print(f"Exception: {e}")
        raise HTTPException(status_code=500, detail=f"Generation failed: {e}")

    # Extract just the text content
    try:
        content = out["choices"][0]["message"]["content"]
    except Exception:
        content = "Error: Could not generate response"

    # Return plain text
    return content

@app.post("/api/chat")
def chat_plain(req: ChatCompletionsRequest):
    """
    Alternative endpoint that returns plain text
    """
    if not req.messages:
        raise HTTPException(status_code=400, detail="messages is required")

    chat = []
    for m in req.messages:
        chat.append({"role": m.role, "content": m.content})

    try:
        out = llm.create_chat_completion(
            messages=chat,
            temperature=req.temperature or 0.2,
            max_tokens=req.max_tokens or 512,
            top_p=req.top_p or 0.95,
            stop=req.stop or None,
        )
        
        content = out["choices"][0]["message"]["content"]
        return content
        
    except Exception as e:
        print(f"Exception: {e}")
        raise HTTPException(status_code=500, detail=f"Generation failed: {e}")

@app.get("/")
def root():
    return {"status": "ok", "message": "Use POST /v1/chat/completions or POST /api/chat for plain text responses"}

if __name__ == "__main__":
    uvicorn.run("server:app", host=HOST, port=PORT, reload=False)