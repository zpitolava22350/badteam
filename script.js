const buttons = document.querySelectorAll('.sliding-button');

buttons.forEach(button => {
  button.addEventListener('mouseenter', () => {
    button.style.transform = 'translateX(100px)';
    switch(button.innerText){
      case "Cube Cavern":
        let temp = document.getElementById("title");
        temp.innerText = button.innerText;
        break:
    }
  });

  button.addEventListener('mouseleave', () => {
    button.style.transform = 'translateX(0)';
  });
});
