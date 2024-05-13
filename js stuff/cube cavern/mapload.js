var hubRoom = JSON.parse(`{"blocks":[{"x":-5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":10,"dy":1,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":-5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0}]}`);

var firstRoom = JSON.parse(`{"blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0}]}`)

//ALL TURN LEFT
var turnRooms = [
    JSON.parse(`{
        "blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":3,"y":3,"z":6,"dx":1,"dy":5,"dz":19,"tex":1,"wrap":1,"tag":0},{"x":-6,"y":3,"z":-3,"dx":19,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":-9,"y":3,"z":3,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":-3,"y":3,"z":9,"dx":1,"dy":5,"dz":13,"tex":1,"wrap":1,"tag":0}],
        "heightDiff": 0
    }`)
];

var straightRooms = [
    JSON.parse(`{
        "blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":-3,"y":3,"z":0,"dx":1,"dy":5,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":3,"y":3,"z":0,"dx":1,"dy":5,"dz":31,"tex":1,"wrap":1,"tag":0}],
        "heightDiff": 0
    }`)
];

/*
var rooms = [
    JSON.parse(`{
        "blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":3,"y":3,"z":6,"dx":1,"dy":5,"dz":19,"tex":1,"wrap":1,"tag":0},{"x":-6,"y":3,"z":-3,"dx":19,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":-9,"y":3,"z":3,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":-3,"y":3,"z":9,"dx":1,"dy":5,"dz":13,"tex":1,"wrap":1,"tag":0}],
        "heightDiff": 0,
        "exit": "left"
    }`),
    JSON.parse(`{
        "blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":-3,"y":3,"z":6,"dx":1,"dy":5,"dz":19,"tex":1,"wrap":1,"tag":0},{"x":9,"y":3,"z":3,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":3,"y":3,"z":9,"dx":1,"dy":5,"dz":13,"tex":1,"wrap":1,"tag":0},{"x":6,"y":3,"z":-3,"dx":19,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0}],
        "heightDiff": 0,
        "exit": "right"
    }`),
    JSON.parse(`{
        "blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":-3,"y":3,"z":0,"dx":1,"dy":5,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":3,"y":3,"z":0,"dx":1,"dy":5,"dz":31,"tex":1,"wrap":1,"tag":0}],
        "heightDiff": 0,
        "exit": "straight"
    }`)
]
*/

function formatMaps(){

    for(let i=0; i<hubRoom.blocks.length; i++){
        hubRoom.blocks[i].tex = "grass";
        delete hubRoom.blocks[i].tag;
    }

    for(let i=0; i<firstRoom.blocks.length; i++){
        delete firstRoom.blocks[i].tex;
        delete firstRoom.blocks[i].tag;
    }

    for(let r=0; r<turnRooms.length; r++){
        for(let b=0; b<turnRooms[r].blocks.length; b++){
            delete turnRooms[r].blocks[b].tag;
            delete turnRooms[r].blocks[b].tex;
        }
    }

    for(let r=0; r<straightRooms.length; r++){
        for(let b=0; b<straightRooms[r].blocks.length; b++){
            delete straightRooms[r].blocks[b].tag;
            delete straightRooms[r].blocks[b].tex;
        }
    }
    
}

function generateDungeon(size, tex){

  let mazeArray = [[1]];
  let currentX = 0;
  let currentY = 0;
  let currentDirection = 0;

  let directionArray = [];

  do {

  } while (directionArray.length < size);

  //------------------------------------

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