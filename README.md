# Neo Space Invaders

A remake of the classic Space Invaders game, featuring adaptive difficulty and AI-generated (at run-time) backgrounds and sprites.

For sample gameplay, see this video: https://youtu.be/9rDBJMO4MdI.

How to run: 
1. In Google Colab, upload the `nsi_genai_api.ipynb` notebook from the `API` directory.
2. In Colab, connect to a compute instance with Nvidia A100 GPU.
3. Upload the other files in the `API` directory to the file system of the Colab compute instance.
4. In the `nsi_genai_api.ipynb` notebook, replace `<AUTH TOKEN>` with your ngrok auth token.
5. Run the Google Colab notebook to start the API, which will generate images using Stable Diffusion v2 and return those images to the game running in Unity vai HTTP/HTTPS. After installing the dependencies, you may have to restart the notebook session before running the following cells.
6. Once the API is running, the output of the final cell should show the public ngrok URL, which will tunnel requests to the API running in the Colab compute instance.
7. To confirm the API is responsive, open up a web browser and go to `<public ngrok URL>/ai_generated_image?text=a%20galaxy%20in%20outer%20space`. There will be a page from ngrok that pops up, asking you to confirm that you want to visit the site. Click the "Visit Site" button to confirm. This only needs to be done the first time a request is sent from the browser to the API. The game code sends requests with a specific header to bypass this page. An AI-generated image of a galaxy should be returned and visible in the browser.
8. Create a project in Unity, and upload the contents of the `Assets` directory here to the `Assets` directory in Unity.
9. In Unity, find the LoadScreenRawImage GameObject in the Hierarchy of the LoadScreen scene. There will be an argument in the Inspector for the Load Screen script, called Genai Api Url. Set the value of that argument to the public ngrok URL. This is how the game knows where to send web requests to get AI-generated images for the background and invader sprites.
10. In Unity, go to `File > Build Settings` and from the Project tab, drag and drop all the `.unity` scene files into the Scenes In Build list, if they are not already there. This ensures that all scenes are included in the build and available to the game while it is running.
11. Press the play button at the top of the Unity window, and enjoy playing the game!
