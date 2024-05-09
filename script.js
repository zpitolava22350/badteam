const buttons = document.querySelectorAll('.sliding-button');

document.addEventListener("DOMContentLoaded", function() {
  buttons.forEach(button => {
    console.log('Button:', button);
    button.addEventListener('mouseenter', () => {
      button.style.transform = 'translateX(100px)';
      let temp = document.getElementById("title");
      console.log(button.innerText);
      console.log(temp.innerText);
      temp.innerText = button.innerText;
    });
  
    button.addEventListener('mouseleave', () => {
      button.style.transform = 'translateX(0)';
    });
  });
});
