const raycaster = new THREE.Raycaster();

function raycast() {

  raycaster.setFromCamera(new THREE.Vector2(0, 0), camera);
  return raycaster.intersectObjects(scene.children, true)[0];

}