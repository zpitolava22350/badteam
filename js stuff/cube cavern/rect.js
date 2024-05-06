class Rect{
  constructor(x,y,z,dx,dy,dz,tex,wrap){
    this.x = x;
    this.y = y;
    this.z = z;
    this.dx = dx;
    this.dy = dy;
    this.dz = dz;
    this.wrap = wrap;

    let geometry = new THREE.BoxGeometry(this.dx, this.dy, this.dz);
    let material = [];
    let texture;

    for(let f=0; f<6; f++){

      texture = tex;
      
      texture.magFilter = THREE.NearestFilter;
      texture.minFilter = THREE.LinearMipmapLinearFilter;
      texture.wrapS = THREE.RepeatWrapping;
      texture.wrapT = THREE.RepeatWrapping;
      if(f === 0 || f === 1){
        texture.repeat.set( (this.dz / this.wrap), (this.dy / this.wrap) );
      }
      if(f === 2 || f === 3){
        texture.repeat.set( (this.dx / this.wrap), (this.dz / this.wrap) );
      }
      if(f === 4 || f === 5){
        texture.repeat.set( (this.dx / this.wrap), (this.dy / this.wrap) );
      }
      
      material.push(new THREE.MeshStandardMaterial( { map: texture, side: THREE.DoubleSide, shadowSide:THREE.FrontSide} ));
    }

    this.cube = new THREE.Mesh(geometry, material);
    //this.cube.castShadow = true;
    //this.cube.receiveShadow = true;
    
    this.cube.position.x = this.x;
    this.cube.position.y = this.y;
    this.cube.position.z = this.z;
    
    scene.add(this.cube);
    
  }

  collideFloor(){
    
    let inside;
    
    if(player.x - halfWidth < this.x + (this.dx*0.5) && player.x + halfWidth > this.x - (this.dx*0.5) && player.z - halfWidth < this.z + (this.dz*0.5) && player.z + halfWidth > this.z - (this.dz*0.5)){
      inside = true;
    }

    if(inside){
      
      if(player.y - halfHeight > this.y + (this.dy*0.5) && player.y - halfHeight + (player.yVel*deltaTime) < this.y + (this.dy*0.5)){
        //above, but hit ground next frame
        player.y = this.y + (this.dy*0.5) + halfHeight + 0.0000001;
        player.yVel = 0;
        player.onGround = true;
      }

      if(player.y + halfHeight < this.y - (this.dy*0.5) && player.y + halfHeight + (player.yVel*deltaTime) > this.y - (this.dy*0.5)){
        //under, but hit head next frame
        player.y = this.y - (this.dy*0.5) - halfHeight - 0.0000001;
        player.yVel = 0;
      }
      
    }
    
  }

  collide(h){

    let inY = false;
    let canStep = false;
    if(player.y - halfHeight + (player.yVel*deltaTime) < this.y + (this.dy*0.5) && player.y + halfHeight + (player.yVel*deltaTime) > this.y - (this.dy*0.5)){
      inY = true;
      if( (this.y + (this.dy*0.5)) - (player.y - halfHeight) <= stepHeight){
        canStep = true;
      }
    }

    if(inY){
      
      let inX = false;
      let inXNext = false;
      let inZ = false;
      let inZNext = false;

      if(player.x + halfWidth > this.x - (this.dx*0.5) && player.x - halfWidth < this.x + (this.dx*0.5)){
        inX = true;
      }
      if(player.z + halfWidth > this.z - (this.dz*0.5) && player.z - halfWidth < this.z + (this.dz*0.5)){
        inZ = true;
      }
      if(player.x + halfWidth + (player.xVel*deltaTime) > this.x - (this.dx*0.5) && player.x - halfWidth + (player.xVel*deltaTime) < this.x + (this.dx*0.5)){
        inXNext = true;
      }
      if(player.z + halfWidth + (player.zVel*deltaTime) > this.z - (this.dz*0.5) && player.z - halfWidth + (player.zVel*deltaTime) < this.z + (this.dz*0.5)){
        inZNext = true;
      }

      if(inZ && !inX && inXNext){
        if(canStep && player.onGround){
          player.y = this.y + (this.dy*0.5) + halfHeight + 0.0000001;
        } else {
          if(player.x < this.x){
            player.x = this.x - (this.dx*0.5) - halfWidth;
            player.xVel = 0;
          }
          if(player.x > this.x){
            player.x = this.x + (this.dx*0.5) + halfWidth;
            player.xVel = 0;
          }
        }
      }

      if(inX && !inZ && inZNext){
        if(canStep && player.onGround){
          player.y = this.y + (this.dy*0.5) + halfHeight + 0.0000001;
        } else {
          if(player.z < this.z){
            player.z = this.z - (this.dz*0.5) - halfWidth;
            player.zVel = 0;
          }
          if(player.z > this.z){
            player.z = this.z + (this.dz*0.5) + halfWidth;
            player.zVel = 0;
          }
        }
      }

      //bugfix
      if(!inX && !inZ && inXNext && inZNext){
        if(Math.abs(player.xVel) > Math.abs(player.zVel)){
          if(player.z < this.z){
            player.z = this.z - (this.dz*0.5) - halfWidth;
            player.zVel = 0;
          }
          if(player.z > this.z){
            player.z = this.z + (this.dz*0.5) + halfWidth;
            player.zVel = 0;
          }
        } else {
          if(player.x < this.x){
            player.x = this.x - (this.dx*0.5) - halfWidth;
            player.xVel = 0;
          }
          if(player.x > this.x){
            player.x = this.x + (this.dx*0.5) + halfWidth;
            player.xVel = 0;
          }
        }
      }
      
    }
    
    
  }
  
}