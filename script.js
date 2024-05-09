const buttons = document.querySelectorAll('.sliding-button');

document.addEventListener("DOMContentLoaded", function() {
  buttons.forEach(button => {
    console.log('Button:', button);
    button.addEventListener('mouseenter', () => {
      button.style.transform = 'translateX(100px)';
      document.getElementById("title").innerText = document.querySelector("button").innerText
    });
  
    button.addEventListener('mouseleave', () => {
      button.style.transform = 'translateX(0)';
    });
  });
});
