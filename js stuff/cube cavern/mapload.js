var hubRoom = JSON.parse(`{"blocks":[{"x":-5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":10,"dy":1,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":-5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0}]}`);

var firstRoom = JSON.parse(`{"blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0}]}`);

//ALL TURN LEFT
var turnRoomsLoad = [
    JSON.parse(`{"blocks":[[0,31,6,1,0,31,"stone",1,""],[0,31,3,25,-15,1,"stone",1,""],[9,13,3,25,15,1,"stone",1,""],[-9,13,3,25,15,1,"stone",1,""],[-15,1,3,25,9,13,"stone",1,""],[15,1,3,25,0,31,"stone",1,""],[0,5,10.5,10,15,1,"stone",1,""],[0,5,0,1,15,1,"grass",1,""],[0,5,-5,9,15,1,"stone",1,""],[-15,1,3,25,-9,13,"stone",1,""],[-15,1,11,9,0,5,"stone",1,""],[0,31,0,1,0,31,"grass",1,""],[-15,1,0,1,0,5,"grass",1,""],[-15,1,-5,9,0,5,"stone",1,""]],"markers":[[0,1,0,"crafting"],[6,1,0,"enemy"],[-6,1,0,"item"]]}`)
];

var straightRoomsLoad = [
    JSON.parse(`{"blocks":[[0,31,6,1,0,31,"stone",1,""],[0,31,0,1,0,31,"grass",1,""],[9,13,3,25,-15,1,"stone",1,""],[-9,13,3,25,-15,1,"stone",1,""],[9,13,3,25,15,1,"stone",1,""],[-9,13,3,25,15,1,"stone",1,""],[-15,1,3,25,0,31,"stone",1,""],[15,1,3,25,0,31,"stone",1,""],[0,5,10.5,10,-15,1,"stone",1,""],[0,5,0,1,-15,1,"grass",1,""],[0,5,10.5,10,15,1,"stone",1,""],[0,5,-5,9,-15,1,"stone",1,""],[0,5,0,1,15,1,"grass",1,""],[0,5,-5,9,15,1,"stone",1,""]],"markers":[[6,1,0,"enemy"],[-6,1,0,"item"],[0,1,0,"crafting"]]}
    `)
];

var turnRooms = [];
var straightRooms = [];

function formatMaps(){

    for(let i=0; i<hubRoom.blocks.length; i++){
        hubRoom.blocks[i].tex = "grass";
        delete hubRoom.blocks[i].tag;
    }

    for(let i=0; i<firstRoom.blocks.length; i++){
        delete firstRoom.blocks[i].tex;
        delete firstRoom.blocks[i].tag;
    }

    for(let r=0; r<turnRoomsLoad.length; r++){
      turnRooms.push({blocks: [], enemies: [], items: [], crafting: []});
        for(let b=0; b<turnRoomsLoad[r].blocks.length; b++){
          turnRooms[r].blocks.push({
            x: turnRoomsLoad[r].blocks[b][0],
            y: turnRoomsLoad[r].blocks[b][2],
            z: turnRoomsLoad[r].blocks[b][4],
            dx: turnRoomsLoad[r].blocks[b][1],
            dy: turnRoomsLoad[r].blocks[b][3],
            dz: turnRoomsLoad[r].blocks[b][5],
            tex: null,
            wrap: turnRoomsLoad[r].blocks[b][7],
            data: turnRoomsLoad[r].blocks[b][8]
          })
          switch(turnRoomsLoad[r].blocks[b][6]){
            case "grass":
              turnRooms[r].blocks[b].tex = "floor";
              break;
            case "stone":
              turnRooms[r].blocks[b].tex = "wall";
              break;
          }
        }
        console.log(turnRoomsLoad[0])
        for(let m=0; m<turnRoomsLoad[r].markers.length; m++){
          switch(turnRoomsLoad[r].markers[m][3]){
            case "item":
              turnRooms[r].items.push({
                x: turnRoomsLoad[r].markers[m][0],
                y: turnRoomsLoad[r].markers[m][1],
                z: turnRoomsLoad[r].markers[m][2]
              });
              break;
            case "enemy":
              turnRooms[r].enemies.push({
                x: turnRoomsLoad[r].markers[m][0],
                y: turnRoomsLoad[r].markers[m][1],
                z: turnRoomsLoad[r].markers[m][2]
              });
              break;
            case "crafting":
              turnRooms[r].crafting.push({
                x: turnRoomsLoad[r].markers[m][0],
                y: turnRoomsLoad[r].markers[m][1],
                z: turnRoomsLoad[r].markers[m][2]
              });
              break;
          }
        }
    }

    for(let r=0; r<straightRoomsLoad.length; r++){
      straightRooms.push({blocks: [], enemies: [], items: [], crafting: []});
        for(let b=0; b<straightRoomsLoad[r].blocks.length; b++){
          straightRooms[r].blocks.push({
            x: straightRoomsLoad[r].blocks[b][0],
            y: straightRoomsLoad[r].blocks[b][2],
            z: straightRoomsLoad[r].blocks[b][4],
            dx: straightRoomsLoad[r].blocks[b][1],
            dy: straightRoomsLoad[r].blocks[b][3],
            dz: straightRoomsLoad[r].blocks[b][5],
            tex: null,
            wrap: straightRoomsLoad[r].blocks[b][7],
            data: straightRoomsLoad[r].blocks[b][8]
          })
          switch(straightRoomsLoad[r].blocks[b][6]){
            case "grass":
              straightRooms[r].blocks[b].tex = "floor";
              break;
            case "stone":
              straightRooms[r].blocks[b].tex = "wall";
              break;
          }
        }
        for(let m=0; m<straightRoomsLoad[r].markers.length; m++){
          switch(straightRoomsLoad[r].markers[m][3]){
            case "item":
              straightRooms[r].items.push({
                x: straightRoomsLoad[r].markers[m][0],
                y: straightRoomsLoad[r].markers[m][1],
                z: straightRoomsLoad[r].markers[m][2]
              });
              break;
            case "enemy":
              straightRooms[r].enemies.push({
                x: straightRoomsLoad[r].markers[m][0],
                y: straightRoomsLoad[r].markers[m][1],
                z: straightRoomsLoad[r].markers[m][2]
              });
              break;
            case "crafting":
              straightRooms[r].crafting.push({
                x: straightRoomsLoad[r].markers[m][0],
                y: straightRoomsLoad[r].markers[m][1],
                z: straightRoomsLoad[r].markers[m][2]
              });
              break;
          }
        }
    }
    
}

function createRooms(layout, tex){

  let currentRotation = 0;
  for(let r=0; r<layout.length; r++){
    let room;
    let spawnItems = [];
    let spawnEnemies = [];
    let spawnCrafting = [];
    if(r === 0){ // START
      room = [];
      for(let i=0; i<firstRoom.blocks.length; i++){
        room.push({
          x: firstRoom.blocks[i].x,
          y: firstRoom.blocks[i].y,
          z: firstRoom.blocks[i].z,
          dx: firstRoom.blocks[i].dx,
          dy: firstRoom.blocks[i].dy,
          dz: firstRoom.blocks[i].dz,
          tex: tex,
          wrap: firstRoom.blocks[i].wrap
        });
      }
    } else if(r === layout.length-1){ // END
      room = [];
      for(let i=0; i<firstRoom.blocks.length; i++){
        room.push({
          x: firstRoom.blocks[i].x,
          y: firstRoom.blocks[i].y,
          z: -firstRoom.blocks[i].z,
          dx: firstRoom.blocks[i].dx,
          dy: firstRoom.blocks[i].dy,
          dz: firstRoom.blocks[i].dz,
          tex: tex,
          wrap: firstRoom.blocks[i].wrap
        });
      }
    } else { // ROOMS
      let rnd;
      switch(layout[r].dir){
        case 0:
          rnd = Math.floor(Math.random()*(straightRooms.length-1));

          room = JSON.parse(JSON.stringify(straightRooms[rnd].blocks));
          spawnItems = JSON.parse(JSON.stringify(straightRooms[rnd].items));
          spawnCrafting = JSON.parse(JSON.stringify(straightRooms[rnd].crafting));
          spawnEnemies = JSON.parse(JSON.stringify(straightRooms[rnd].enemies));
          break;
        case 1:
          rnd = Math.floor(Math.random()*(turnRooms.length-1));

          room = JSON.parse(JSON.stringify(turnRooms[rnd].blocks));
          spawnItems = JSON.parse(JSON.stringify(turnRooms[rnd].items));
          spawnCrafting = JSON.parse(JSON.stringify(turnRooms[rnd].crafting));
          spawnEnemies = JSON.parse(JSON.stringify(turnRooms[rnd].enemies));
          for(let i=0; i<room.length; i++){
            room[i].x = -room[i].x;
          }
          for(let i=0; i<spawnItems.length; i++){
            spawnItems[i].x = -spawnItems[i].x;
          }
          for(let i=0; i<spawnCrafting.length; i++){
            spawnCrafting[i].x = -spawnCrafting[i].x;
          }
          for(let i=0; i<spawnEnemies.length; i++){
            spawnEnemies[i].x = -spawnEnemies[i].x;
          }
          break;
        case 3:
          rnd = Math.floor(Math.random()*(turnRooms.length-1));

          room = JSON.parse(JSON.stringify(turnRooms[rnd].blocks));
          spawnItems = JSON.parse(JSON.stringify(turnRooms[rnd].items));
          spawnCrafting = JSON.parse(JSON.stringify(turnRooms[rnd].crafting));
          spawnEnemies = JSON.parse(JSON.stringify(turnRooms[rnd].enemies));
          break;
      }
    }
    
    room = rotateRoom(room, currentRotation);

    for(let b=0; b<room.length; b++){
      blocks.push(new Rect(room[b].x + (layout[r].x*31), room[b].y, room[b].z + (layout[r].y*31), room[b].dx, room[b].dy, room[b].dz, tex, room[b].wrap));
    }
    console.log(spawnItems[0])
    for(let i=0; i<spawnItems.length; i++){
      items.push(new Item("random", spawnItems[i].x + (layout[r].x*31), spawnItems[i].y, spawnItems[i].z + (layout[r].y*31)));
    }
    for(let c=0; c<spawnCrafting.length; c++){
      craftingTables.push(new CraftingTable(spawnCrafting[c].x + (layout[r].x*31), spawnCrafting[c].y, spawnCrafting[c].z + (layout[r].y*31)));
    }

    currentRotation = (currentRotation + layout[r].dir)%4;
  }

}

function rotateRoom(roomArr, rotateDir){
  for(let b=0; b<roomArr.length; b++){
    switch(rotateDir){
      case 1:
        [roomArr[b].x, roomArr[b].z] = [-roomArr[b].z, roomArr[b].x];
        [roomArr[b].dx, roomArr[b].dz] = [roomArr[b].dz, roomArr[b].dx];
        break;
      case 2:
        roomArr[b].x = -roomArr[b].x;
        roomArr[b].z = -roomArr[b].z;
        break;
      case 3:
        [roomArr[b].x, roomArr[b].z] = [roomArr[b].z, -roomArr[b].x];
        [roomArr[b].dx, roomArr[b].dz] = [roomArr[b].dz, roomArr[b].dx];
        break;
    }
  }
  return roomArr;
}

function generateDungeon(size, tex){

  let maze = {
    array: [],
    final: [],
    width: 21,
    height: 21,
    centerX: 10,
    centerY: 10,
    currentX: 10,
    currentY: 9,
    mode: "search"
  }

  maze.final.push({x:10, y:10, dir:0});
  maze.final.push({x:10, y:9, dir:null});
  
  let rnd;

  for(let x=0; x<maze.width; x++){
    maze.array.push([]);
    for(let y=0; y<maze.height; y++){
      if(x === maze.centerX && y === maze.centerY){
        maze.array[x].push({dir:0, rel:0, prev:0, av: false});
      } else if (x === maze.centerX && y === maze.centerY-1){
        maze.array[x].push({prev: 0, av: false});
      } else {
        maze.array[x].push({av: true});
      }
    }
  }

  do{

    let canUp = true;
    let canRight = true;
    let canDown = true;
    let canLeft = true;
    if(maze.currentX === 0){
      canLeft = false;
    } else if(!maze.array[maze.currentX-1][maze.currentY].av){
      canLeft = false;
    }
    if(maze.currentX === maze.width-1){
      canRight = false;
    } else if(!maze.array[maze.currentX+1][maze.currentY].av){
      canRight = false;
    }
    if(maze.currentY === 0){
      canUp = false;
    } else if(!maze.array[maze.currentX][maze.currentY-1].av){
      canUp = false;
    }
    if(maze.currentY === maze.height-1){
      canDown = false;
    } else if(!maze.array[maze.currentX][maze.currentY+1].av){
      canDown = false;
    }

    if(canUp || canDown || canLeft || canRight){
      rnd = [0, 1, 2, 3];
      rnd = shuffle(rnd);
      let valid = false;
      do {
        switch((maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4){
          case 0:
            if(canUp){
              maze.array[maze.currentX][maze.currentY].dir = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.array[maze.currentX][maze.currentY].rel = rnd[0];
              maze.final[maze.final.length-1].dir = rnd[0];
              maze.array[maze.currentX][maze.currentY-1].prev = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.currentY--;
              maze.array[maze.currentX][maze.currentY].av = false;
              maze.final.push({x:maze.currentX, y:maze.currentY, dir:null});
              valid = true;
            }
            break;
          case 1:
            if(canRight){
              maze.array[maze.currentX][maze.currentY].dir = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.array[maze.currentX][maze.currentY].rel = rnd[0];
              maze.final[maze.final.length-1].dir = rnd[0];
              maze.array[maze.currentX+1][maze.currentY].prev = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.currentX++;
              maze.array[maze.currentX][maze.currentY].av = false;
              maze.final.push({x:maze.currentX, y:maze.currentY, dir:null});
              valid = true;
            }
            break;
          case 2:
            if(canDown){
              maze.array[maze.currentX][maze.currentY].dir = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.array[maze.currentX][maze.currentY].rel = rnd[0];
              maze.final[maze.final.length-1].dir = rnd[0];
              maze.array[maze.currentX][maze.currentY+1].prev = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.currentY++;
              maze.array[maze.currentX][maze.currentY].av = false;
              maze.final.push({x:maze.currentX, y:maze.currentY, dir:null});
              valid = true;
            }
            break;
          case 3:
            if(canLeft){
              maze.array[maze.currentX][maze.currentY].dir = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.array[maze.currentX][maze.currentY].rel = rnd[0];
              maze.final[maze.final.length-1].dir = rnd[0];
              maze.array[maze.currentX-1][maze.currentY].prev = (maze.array[maze.currentX][maze.currentY].prev + rnd[0])%4;
              maze.currentX--;
              maze.array[maze.currentX][maze.currentY].av = false;
              maze.final.push({x:maze.currentX, y:maze.currentY, dir:null});
              valid = true;
            }
            break;
        }
        rnd.pop();
      } while (rnd.length >= 1 && !valid);
    } else {
      maze.final.pop();
      switch(maze.array[maze.currentX][maze.currentY].prev){
        case 0:
          maze.currentY++;
          break;
        case 1:
          maze.currentX--;
          break;
        case 2:
          maze.currentY--;
          break;
        case 3:
          maze.currentX++;
          break;
      }
    }
  } while (maze.final.length <= size+1);
  maze.final[maze.final.length-1].dir = 0;

  for(let i=0; i<maze.final.length; i++){
    maze.final[i].x -= 10;
    maze.final[i].y -= 10;
  }

  createRooms(maze.final, tex);

}

function shuffle(array) {
  for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
  }
  return array;
}