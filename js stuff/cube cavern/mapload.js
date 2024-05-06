var hubRoom = JSON.parse(`{"blocks":[{"x":-5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":10,"dy":1,"dz":10,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":-5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":10,"dy":6,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":0,"dx":1,"dy":6,"dz":10,"tex":1,"wrap":1,"tag":0}]}`);

var firstRoom = JSON.parse(`{"blocks":[{"x":0,"y":6,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":0,"y":0,"z":0,"dx":31,"dy":1,"dz":31,"tex":1,"wrap":1,"tag":0},{"x":6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":0,"y":3,"z":5,"dx":13,"dy":5,"dz":1,"tex":1,"wrap":1,"tag":0},{"x":5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-6,"y":3,"z":0,"dx":1,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0},{"x":-5,"y":3,"z":-10,"dx":5,"dy":5,"dz":11,"tex":1,"wrap":1,"tag":0}]}`)

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
    }`)
]

function formatMaps(){

    for(let i=0; i<hubRoom.blocks.length; i++){
        hubRoom.blocks[i].tex = "grass";
        delete hubRoom.blocks[i].tag;
    }

    for(let i=0; i<firstRoom.blocks.length; i++){
        delete firstRoom.blocks[i].tex;
        delete firstRoom.blocks[i].tag;
    }

    for(let r=0; r<rooms.length; r++){
        for(let b=0; b<rooms[r].blocks.length; b++){
            delete firstRoom.blocks[b].tag;
        }
    }
    
}