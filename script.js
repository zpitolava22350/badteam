const buttons = document.querySelectorAll('.sliding-button');

buttons.forEach(button => {
  button.addEventListener('mouseenter', () => {
    button.style.transform = 'translateX(100px)';
  });

  button.addEventListener('mouseleave', () => {
    button.style.transform = 'translateX(0)';
  });
});
