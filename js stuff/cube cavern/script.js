const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera( 100, window.innerWidth / window.innerHeight, 0.01, 50 );
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

scene.fog = new THREE.Fog( 0xa0c8ff, 0, 50 );

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
    head: null,
    back: null,
    feet: null
  }
}

const playerHeight = 1.8;
const playerWidth = playerHeight * 0.3;
const halfHeight = playerHeight * 0.5;
const halfWidth = playerWidth * 0.5;
const stepHeight = 0.6;

const speed = 0.00027;
const gravity = 0.000024;
const jumpHeight = 0.009;
const dampening = 0.012;

let t1 = 0;
let t2 = 0;

function animate(){
  requestAnimationFrame(animate);

  camera.position.x = player.x;
  camera.position.y = player.y + (halfHeight/2);
  camera.position.z = player.z;
  playerPointLight.position.set( player.x, player.y + (halfHeight/2), player.z );
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();

  renderer.render(scene, camera);
}

animate();

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

var keyState = {
  q: false
}

var hud = {
  cursor: {
    x:0,
    y:0
  },
  menu: "none"
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
  frameRate(999999);

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
  items.push(new Item("Rock", 2, 1, -3));

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

  if(keyIsDown(81) && !keyState.q){
    if(hud.menu === "none"){
      hud.cursor.x = window.innerWidth/2;
      hud.cursor.y = window.innerHeight/2;
      hud.menu = "drop";
    }
    hud.cursor.x += movedX;
    hud.cursor.y += movedY;
    if(hud.cursor.x >= (window.innerWidth/2) - 145 && hud.cursor.x <= (window.innerWidth/2) - 55 && hud.cursor.y >= (window.innerHeight/2) - 45 && hud.cursor.y <= (window.innerHeight/2) + 45 && hud.menu != "none"){
      //back
      if(player.inventory.back != null){
        items.push(new Item(player.inventory.back.item, player.x, player.y, player.z));
        player.inventory.back = null;
      }
      hud.menu = "none";
      keyState.q = true;
      updateCraftingTables();
    }
    if(hud.cursor.x >= (window.innerWidth/2) - 45 && hud.cursor.x <= (window.innerWidth/2) + 45 && hud.cursor.y >= (window.innerHeight/2) - 145 && hud.cursor.y <= (window.innerHeight/2) - 55 && hud.menu != "none"){
      //head
      if(player.inventory.head != null){
        items.push(new Item(player.inventory.head.item, player.x, player.y, player.z));
        player.inventory.head = null;
      }
      hud.menu = "none";
      keyState.q = true;
      updateCraftingTables();
    }
    if(hud.cursor.x >= (window.innerWidth/2) - 45 && hud.cursor.x <= (window.innerWidth/2) + 45 && hud.cursor.y >= (window.innerHeight/2) + 55 && hud.cursor.y <= (window.innerHeight/2) + 145 && hud.menu != "none"){
      //feet
      if(player.inventory.feet != null){
        items.push(new Item(player.inventory.feet.item, player.x, player.y, player.z));
        player.inventory.feet = null;
      }
      hud.menu = "none";
      keyState.q = true;
      updateCraftingTables();
    }
    if(hud.cursor.x >= (window.innerWidth/2) + 55 && hud.cursor.x <= (window.innerWidth/2) + 145 && hud.cursor.y >= (window.innerHeight/2) - 45 && hud.cursor.y <= (window.innerHeight/2) + 45 && hud.menu != "none"){
      //hand
      if(player.inventory.slot[player.inventory.slotSelected] != null){
        items.push(new Item(player.inventory.slot[player.inventory.slotSelected].item, player.x, player.y, player.z));
        player.inventory.slot[player.inventory.slotSelected] = null;
      }
      hud.menu = "none";
      keyState.q = true;
      updateCraftingTables();
    }
  } else {
    hud.menu = "none";
    /*
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
    */
  }

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

function keyReleased(){
  switch(keyCode){
    case 81:
      keyState.q = false;
      break;
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
  textSize(16);

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

  clr = color(50);
  clr.setAlpha(180);
  fill(clr);
  textSize(24);

  if(hud.menu === "drop"){
    rect((window.innerWidth/2) - 150, (window.innerHeight/2) - 150, 300, 300);
    fill(200);
    rect((window.innerWidth/2) - 145, (window.innerHeight/2) - 45, 90, 90);
    rect((window.innerWidth/2) - 45, (window.innerHeight/2) - 145, 90, 90);
    rect((window.innerWidth/2) - 45, (window.innerHeight/2) + 55, 90, 90);
    rect((window.innerWidth/2) + 55, (window.innerHeight/2) - 45, 90, 90);

    fill(0);
    textSize(24);
    text("Back", (window.innerWidth/2) - 145, (window.innerHeight/2) - 45, 90, 90);
    text("Head", (window.innerWidth/2) - 45, (window.innerHeight/2) - 145, 90, 90);
    text("Feet", (window.innerWidth/2) - 45, (window.innerHeight/2) + 55, 90, 90);
    text("Hand", (window.innerWidth/2) + 55, (window.innerHeight/2) - 45, 90, 90);
  }

  if(hud.menu != "none"){
    clr = color(0);
    clr.setAlpha(150);
    fill(clr);
    stroke(0);
    strokeWeight(1);
    circle(hud.cursor.x, hud.cursor.y, 6);
  }

}

function updateCraftingTables(){
  for(let c=0; c<craftingTables.length; c++){
    craftingTables[c].check();
  }
}

function itemRemove(itemIndex){
  items[itemIndex].unload();
  items.splice(itemIndex, 1);
  updateItemIndexes();
}

function itemsRemove(indexArray){
  indexArray.sort((a, b) => a - b);
  let lgth = indexArray.length;
  for(let k=lgth-1; k>=0; k--){
    itemRemove(indexArray[k]);
    indexArray.splice(k, 1);
  }
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
      clearOther();
      generateDungeon(8, "grass");
      break;
  }
  player.x = 0;
  player.y = 2;
  player.z = 0;
  player.xVel = 0;
  player.yVel = 0;
  player.zVel = 0;
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

function clearOther(){
  for(let i=0; i<items.length; i++){
    items[i].unload();
  }
  for(let i=0; i<enemies.length; i++){
    //enemies[i].unload();
  }
  for(let i=0; i<craftingTables.length; i++){
    craftingTables[i].unload();
  }
  items.length = 0;
  enemies.length = 0;
  craftingTables.length = 0;
}

let isLocked = false;

document.addEventListener('click', () => {
    if (!isLocked) {
        document.body.requestPointerLock();
    }
});

document.addEventListener('pointerlockchange', () => {
    isLocked = !!document.pointerLockElement;
});

let mouseMovementX = 0;
let mouseMovementY = 0;
let prevMouseX = 0;
let prevMouseY = 0;

document.addEventListener('mousemove', (event) => {
    if (isLocked && hud.menu === "none") {
        mouseMovementX = event.movementX || event.mozMovementX || event.webkitMovementX || 0;
        mouseMovementY = event.movementY || event.mozMovementY || event.webkitMovementY || 0;

        mouseMovementX = Math.min(Math.abs(mouseMovementX), Math.abs(prevMouseX)+10, 100)*Math.sign(mouseMovementX);
        mouseMovementY = Math.min(Math.abs(mouseMovementY), Math.abs(prevMouseY)+10, 100)*Math.sign(mouseMovementY);

        player.r -= mouseMovementX * 0.003;
        player.t -= mouseMovementY * 0.003;

        player.t = Math.max(-Math.PI / 2, Math.min(Math.PI / 2, player.t));

        if (player.r > Math.PI) {
            player.r -= Math.PI * 2;
        } else if (player.r < -Math.PI) {
            player.r += Math.PI * 2;
        }

        camera.rotation.set(0, 0, 0);
        camera.rotateY(player.r);
        camera.rotateX(player.t);

        prevMouseX = mouseMovementX;
        prevMouseY = mouseMovementY;
    }
});