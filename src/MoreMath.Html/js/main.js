function toggle_navbar() {
  const navbar = document.querySelector(".navbar__content");
  navbar.classList.toggle("visible");
}
function close_navbar() {
  const navbar = document.querySelector(".navbar__content");
  navbar.classList.remove("visible");
}
