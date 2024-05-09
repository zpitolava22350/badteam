const buttons = document.querySelectorAll('.sliding-button');

window.onload = function(e){
  buttons.forEach(button => {
    button.addEventListener('mouseenter', () => {
      document.getElementById("title").innerText = button.innerText;
      var description = document.getElementById("description");
      switch(button.innerText){
        case "Cube Cavern":
          description.innerText = "a remake of a roblox game lmao";
          break;
        case "GitHub Pages":
          description.innerText = "where you are now";
          break;
      }
    });
  });
};

console.log("Loaded!");
