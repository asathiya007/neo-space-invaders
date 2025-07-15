from diffusers import StableDiffusionPipeline
from fastapi import FastAPI
from fastapi.responses import Response
import io
from PIL import Image, ImageDraw

from pyngrok import ngrok
import threading
import torch
import uvicorn


# utility function to apply circle crop to image
def circle_crop(img):
    # convert to RGBA for transparency
    img = img.convert('RGBA')

    # create mask (black background, white circle)
    mask = Image.new('L', img.size, 0)
    draw = ImageDraw.Draw(mask)
    width, height = img.size
    radius = min(width, height) // 2
    center = (width // 2, height // 2)
    draw.ellipse(
        (center[0] - radius, center[1] - radius, center[0] + radius,
         center[1] + radius),
        fill=255
    )

    # apply mask to image (everything outside circle becomes transparent)
    new_img = Image.new('RGBA', img.size)
    new_img.paste(img, (0, 0), mask)

    return new_img


# set up pipeline for Stable Diffusion (using fp16 for faster inference)
pipeline = StableDiffusionPipeline.from_pretrained(
    'stabilityai/stable-diffusion-2',
    revision='fp16', torch_dtype=torch.float16)
pipeline = pipeline.to(
    torch.device('cuda' if torch.cuda.is_available() else 'cpu'))

# create FastAPI API
api = FastAPI()
PORT = 8000


# endpoint to get AI-generated image
@api.get('/ai_generated_image')
def get_image(text, crop='square'):
    img = pipeline(text).images[0]
    # apply circle crop, if specified
    if crop == 'circle':
        img = circle_crop(img)
    buf = io.BytesIO()
    img.save(buf, format='PNG')
    img_bytes = buf.getvalue()
    return Response(content=img_bytes, media_type='image/png')


# start ngrok tunnel
public_url = ngrok.connect(PORT)
print(f'Public URL: {public_url}')


# function to run API
def run():
    uvicorn.run(api, host='0.0.0.0', port=PORT)


# run API
threading.Thread(target=run).start()
