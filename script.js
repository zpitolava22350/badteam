const buttons = document.querySelectorAll('.sliding-button');

window.onload = function(e){
  buttons.forEach(button => {
    button.addEventListener('mouseenter', () => {
      document.getElementById("title").innerText = button.innerText;
      var description = document.getElementById("description");
      switch(button.innerText){
        case "Minecraft 2 lol":
          description.innerText = "Chunk based voxel engine + breaking blocks";
          break;
        case "Build to Survive":
          description.innerText = "unfinished base building game";
          break;
        case "Cube Cavern":
          description.innerText = "a remake of a roblox game lmao (not done lmfao)";
          break;
        case "GitHub Pages":
          description.innerText = "where you are now";
          break;
      }
    });
  });
};
