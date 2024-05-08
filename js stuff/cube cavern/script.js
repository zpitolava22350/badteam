const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera( 100, window.innerWidth / window.innerHeight, 0.01, 80 );
const renderer = new THREE.WebGLRenderer({antialias:true});
//renderer.shadowMap.enabled = true;
//renderer.shadowMap.type = THREE.PCFSoftShadowMap;
//renderer.shadowMap.autoUpdate = false;

renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild( renderer.domElement );

scene.background = new THREE.Color( 0xa0c8ff );

const ambientLight = new THREE.AmbientLight( 0xffffff, 0.5 );
scene.add( ambientLight );

const playerPointLight = new THREE.PointLight( 0xffffff, 0.2 );
scene.add( playerPointLight );

function animate(){
  requestAnimationFrame(animate);
  renderer.render(scene, camera);
}

animate();

const playerHeight = 1.8;
const playerWidth = playerHeight * 0.3;
const halfHeight = playerHeight * 0.5;
const halfWidth = playerWidth * 0.5;
const stepHeight = 0.6;

const speed = 0.00007;
const gravity = 0.000024;
const jumpHeight = 0.009;
const dampening = 0.012;

var genPos = {
  x: 0,
  y: 0,
  z: 0,
  r: 0
}

var area = {
  name: "hub",
  boss: false
};

var player = {
  x: 0,
  y: 2,
  z: 0,
  xVel: 0,
  yVel: 0,
  zVel: 0,
  r: 0,
  t: 0,
  onGround: false,
  inventory: {
    slotSelected: 0,
    slot: [null, null, null],
    back: null,
    feet: null
  }
}

var textures = {};

var blocks = [];

var items = [];
var enemies = [];
var craftingTables = [];

function setup(){
  var cnv = createCanvas(window.innerWidth, window.innerHeight);
  cnv.position(0,0);
  pixelDensity(1);
  noSmooth();
  frameRate(9999999);

  let texToLoad = ["grass", "stone", "brick", "gray", "log", "smoothstone", "dark_gray", "orange"];

  for(let i=0; i<texToLoad.length; i++){
    textures[texToLoad[i]] = new THREE.TextureLoader().load(`https://cdn.statically.io/gh/zpitolava22350/badteam/main/assets/images/${texToLoad[i]}.png`);
  }

  formatMaps();

  for(let i=0; i<hubRoom.blocks.length; i++){
    blocks.push(new Rect(hubRoom.blocks[i].x, hubRoom.blocks[i].y, hubRoom.blocks[i].z, hubRoom.blocks[i].dx, hubRoom.blocks[i].dy, hubRoom.blocks[i].dz, hubRoom.blocks[i].tex, hubRoom.blocks[i].wrap))
  }

  craftingTables.push(new CraftingTable(0, 1, -3));

  items.push(new Item("Stick", 1, 1, -3));
  items.push(new Item("Stick", 2, 1, -3));

}

function draw(){

  if(deltaTime > 60){
    deltaTime = 60;
  }

  switch (-keyIsDown(87) + keyIsDown(83) + (keyIsDown(65) * 10) + -(keyIsDown(68) * 10) + 11) {
    case 11://no
      break;
    case 10://W
      player.zVel -= (Math.cos(player.r) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r) * speed) * deltaTime;
      break;
    case 20://WD
      player.zVel -= (Math.cos(player.r + (PI * 0.25)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 0.25)) * speed) * deltaTime;
      break;
    case 21://D
      player.zVel -= (Math.cos(player.r + (PI * 0.5)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 0.5)) * speed) * deltaTime;
      break;
    case 22://SD
      player.zVel -= (Math.cos(player.r + (PI * 0.75)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 0.75)) * speed) * deltaTime;
      break;
    case 12://S
      player.zVel -= (Math.cos(player.r + (PI)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI)) * speed) * deltaTime;
      break;
    case 2://SA
      player.zVel -= (Math.cos(player.r + (PI * 1.25)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 1.25)) * speed) * deltaTime;
      break;
    case 1://A
      player.zVel -= (Math.cos(player.r + (PI * 1.5)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 1.5)) * speed) * deltaTime;
      break;
    case 0://WA
      player.zVel -= (Math.cos(player.r + (PI * 1.75)) * speed) * deltaTime;
      player.xVel -= (Math.sin(player.r + (PI * 1.75)) * speed) * deltaTime;
      break;
  }

  camera.rotateX(-player.t);
  camera.rotateY(-player.r);

  let rotateCam = (round(-movedX, 5) * 0.003);
  let tiltCam = (round(movedY, 5) * 0.003);

  player.r += (rotateCam*deltaTime)/8;
  player.t -= (tiltCam*deltaTime)/8;

  if (player.t >= 1.45) {
    player.t = 1.45;
  } else if (player.t <= -1.45) {
    player.t = -1.45;
  }

  if(player.r > Math.PI){
    player.r -= Math.PI*2;
  } else if(player.r < -Math.PI){
    player.r += Math.PI*2;
  }

  camera.rotateY(player.r);
  camera.rotateX(player.t);

  if(keyIsDown(32) && player.onGround){
    player.yVel += jumpHeight;
    player.onGround = false;
  }

  player.onGround = false;
  for(let i=0; i<blocks.length; i++){
    blocks[i].collideFloor();
  }
  for(let i=0; i<craftingTables.length; i++){
    craftingTables[i].collideFloor();
  }
  for(let i=0; i<blocks.length; i++){
    blocks[i].collide(i);
  }
  for(let i=0; i<craftingTables.length; i++){
    craftingTables[i].collide(i);
  }
  

  if(player.xVel != 0){
    player.x += (player.xVel) * deltaTime;
  }
  if(player.zVel != 0){
    player.z += (player.zVel) * deltaTime;
  }
  if(player.yVel != 0){
    player.y += (player.yVel) * deltaTime;
  }

  player.xVel = lerp(player.xVel,0,(deltaTime*dampening));
  player.zVel = lerp(player.zVel,0,(deltaTime*dampening));
  
  if(!isNaN(gravity * deltaTime)){
    if(Math.abs(player.yVel - (gravity * deltaTime)) <= 0.000005){
      player.yVel = 0;
    } else if(Math.abs(gravity * deltaTime) > 0.000006){
      player.yVel -= gravity * deltaTime;
    }
  }

  displayHud();

  camera.position.x = player.x;
  camera.position.y = player.y + (halfHeight/2);
  camera.position.z = player.z;
  playerPointLight.position.set( player.x, player.y + (halfHeight/2), player.z );
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  
}

function windowResized() {
  renderer.setSize(window.innerWidth, window.innerHeight);
  resizeCanvas(window.innerWidth, window.innerHeight);
}

function keyPressed(){
  switch(keyCode){
    case 80:
      enterDungeon("yellow");
      break;
    case 69:
      let ray = raycast();
      if(Object.keys(ray.object.userData).length >= 1){
        switch(ray.object.userData.type){
          case "item":
            pickupItem(ray.object.userData.index);
            break;
        }
      }
  }

  if(keyCode >= 49 && keyCode <= 57){
    if(player.inventory.slot.length >= keyCode-48){
      player.inventory.slotSelected = keyCode-49;
    }
  }
}

function pickupItem(itemIndex){
  if(items[itemIndex].model.userData.slot === "hand"){
    if(player.inventory.slot[player.inventory.slotSelected] === null){
      player.inventory.slot[player.inventory.slotSelected] = items[itemIndex];
      itemRemove(itemIndex);
      return true;
    }
    for(let s=0; s<player.inventory.slot.length; s++){
      if(player.inventory.slot[s] === null){
        player.inventory.slot[s] = items[itemIndex];
        itemRemove(itemIndex);
        return true;
      }
    }
  }
  return false;
}

function mousePressed(){
  requestPointerLock();
}

function displayHud(){

  let clr;

  clear();

  fill(0);
  stroke(0);
  strokeWeight(2);

  line(width/2, height/2, (width/2)-10, (height/2)+10);
  line(width/2, height/2, (width/2)+10, (height/2)+10);
  noStroke();

  clr = color(50);
  clr.setAlpha(150);

  fill(clr);

  rect(window.innerWidth-(player.inventory.slot.length*110)-10, window.innerHeight-120, player.inventory.slot.length*110+10, 120);

  textAlign(CENTER, CENTER);

  for(let s=player.inventory.slot.length-1; s>=0; s--){
    let slot = player.inventory.slot.length-1-s;
    fill(200);
    if(slot === player.inventory.slotSelected){
      rect(window.innerWidth-(s*110)-110, window.innerHeight-110, 100, 100);
    } else {
      rect(window.innerWidth-(s*110)-95, window.innerHeight-95, 70, 70);
    }
    fill(0);
    if(player.inventory.slot[slot] === null){
      text("Empty", window.innerWidth-(s*110)-110, window.innerHeight-110, 100, 100);
    } else {
      text(player.inventory.slot[slot].item, window.innerWidth-(s*110)-110, window.innerHeight-110, 100, 100);
    }
  }

}

function itemRemove(itemIndex){
  items[itemIndex].unload();
  items.splice(itemIndex, 1);
  updateItemIndexes();
}

function updateItemIndexes(){
  for(let i=0; i<items.length; i++){
    items[i].model.userData.index = i;
  }
}

/*
function updateEnemyIndexes(){
  for(let e=0; e<enemies.length; e++){
    enemies[e].model.userData.index = e;
  }
}
*/

function enterDungeon(type){
  switch(type){
    case "yellow":
      clearBlocks();
      generateDungeon(3, "grass");
      break;
  }
  player.x = 0;
  player.y = 2;
  player.z = 0;
  player.xVel = 0;
  player.yVel = 0;
  player.zVel = 0;
}

function generateDungeon(size, tex){
  for(let i=0; i<firstRoom.blocks.length; i++){
    blocks.push(new Rect(firstRoom.blocks[i].x, firstRoom.blocks[i].y, firstRoom.blocks[i].z, firstRoom.blocks[i].dx, firstRoom.blocks[i].dy, firstRoom.blocks[i].dz, tex, firstRoom.blocks[i].wrap));
  }

  genPos.x = 0;
  genPos.y = 0;
  genPos.z = -31;
  genPos.r = 0;

  for(let r=0; r<size; r++){
    let room = Math.floor(Math.random()*rooms.length);
    for(let b=0; b<rooms[room].blocks.length; b++){
      switch(genPos.r){
        case 0:
          blocks.push(new Rect(rooms[room].blocks[b].x + genPos.x, rooms[room].blocks[b].y + genPos.y, rooms[room].blocks[b].z + genPos.z, rooms[room].blocks[b].dx, rooms[room].blocks[b].dy, rooms[room].blocks[b].dz, tex, rooms[room].blocks[b].wrap));
          break;
        case 1:
          blocks.push(new Rect(-rooms[room].blocks[b].z + genPos.x, rooms[room].blocks[b].y + genPos.y, rooms[room].blocks[b].x + genPos.z, rooms[room].blocks[b].dz, rooms[room].blocks[b].dy, rooms[room].blocks[b].dx, tex, rooms[room].blocks[b].wrap));
          break;
        case 2:
          blocks.push(new Rect(-rooms[room].blocks[b].x + genPos.x, rooms[room].blocks[b].y + genPos.y, -rooms[room].blocks[b].z + genPos.z, rooms[room].blocks[b].dx, rooms[room].blocks[b].dy, rooms[room].blocks[b].dz, tex, rooms[room].blocks[b].wrap));
          break;
        case 3:
          blocks.push(new Rect(rooms[room].blocks[b].z + genPos.x, rooms[room].blocks[b].y + genPos.y, -rooms[room].blocks[b].x + genPos.z, rooms[room].blocks[b].dz, rooms[room].blocks[b].dy, rooms[room].blocks[b].dx, tex, rooms[room].blocks[b].wrap));
          break;
      }
    }
    switch(rooms[room].exit){
      case "left":
        genPos.r--;
        if(genPos.r <= -1){
          genPos.r = 3;
        }
        break;
      case "right":
        genPos.r++;
        if(genPos.r >= 4){
          genPos.r = 0;
        }
        break;
    }
    switch(genPos.r){
      case 0:
        genPos.z -= 31;
        break;
      case 1:
        genPos.x += 31;
        break;
      case 2:
        genPos.z += 31;
        break;
      case 3:
        genPos.x -= 31;
    }
  }
}

function clearBlocks(){
  for(let b=0; b<blocks.length; b++){
    scene.remove(blocks[b].cube);
    blocks[b].cube.geometry.dispose();
    if (blocks[b].cube.material instanceof THREE.Material) {
      blocks[b].cube.material.dispose();
    } else if (Array.isArray(blocks[b].cube.material)) {
      blocks[b].cube.material.forEach(material => material.dispose());
    }
    blocks[b].cube = null;
  }
  blocks.length = 0;
}