var spawnableEnemies = [
  "bruh"
];

class Enemy{
  constructor(type, x, y, z, room){

    if(item != "random"){
      this.type = type;
    } else {
      this.type = spawnableEnemies[Math.floor(Math.random()*spawnableEnemies.length)];
    }

    this.x = x;
    this.y = y;
    this.z = z;
    this.room = room;
    this.loaded = true;

    this.model = new THREE.Mesh(new THREE.BoxGeometry(0.7, 1.2, 0.7), new THREE.MeshStandardMaterial( { color: 0xff0000, side: THREE.FrontSide } ));
    
    this.model.position.x = this.x;
    this.model.position.y = this.y;
    this.model.position.z = this.z;
    
    scene.add(this.model);
  }

  load(){
    this.loaded = true;
    this.model.visible = false;
  }
  unload(){
    this.loaded = false;
    this.model.visible = true;
  }
  update(){
    if(this.loaded){
      
    }
  }
}