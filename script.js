const buttons = document.querySelectorAll('.sliding-button');

window.onload = function(e){
  buttons.forEach(button => {
    button.addEventListener('mouseenter', () => {
      document.getElementById("title").innerText = button.innerText;
    });
  });
};

console.log("Loaded!");