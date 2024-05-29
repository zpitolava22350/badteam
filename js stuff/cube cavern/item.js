var spawnableItems = [
    "Stick",
    "Rock",
    "Rope",
    "Leather"
];

class Item{
    constructor(item, x, y, z){

        if(item != "random"){
            this.item = item;
        } else {
            this.item = spawnableItems[Math.floor(Math.random()*spawnableItems.length)];
        }
        
        this.x = x;
        this.y = y;
        this.z = z;

        this.inInventory = false;

        this.model = new THREE.Mesh(new THREE.BoxGeometry(0.1, 0.1, 0.1), new THREE.MeshBasicMaterial( { color: 0xff0000, side: THREE.FrontSide } ));
        
        this.model.position.x = this.x;
        this.model.position.y = this.y;
        this.model.position.z = this.z;
        
        scene.add(this.model);

        this.model.userData = {
            type: "item",
            index: items.length
        };

        this.model.userData.slot = "hand";

        //check for items that go on back/feet

        this.onGround = false;
        this.drop(this.x, this.y, this.z);
    }

    update(){
        if(this.inInventory){
            this.model.visible = false;
        } else {
            this.drop();
            this.model.visible = true;
        }
    }

    unload(){
        scene.remove(this.model);
        this.model.geometry.dispose();
        if (this.model.material instanceof THREE.Material) {
            this.model.material.dispose();
        } else if (Array.isArray(this.model.material)) {
            this.model.material.forEach(material => material.dispose());
        }
        delete this.model;
    }

    drop(x, y, z){
        if(this.onGround){
            return;
        } else {

            let highestPoint = -100000;
            for(let b=0; b<blocks.length; b++){
                let inside;
        
                if(this.x - 0.05 < blocks[b].x + (blocks[b].dx*0.5) && this.x + 0.05 > blocks[b].x - (blocks[b].dx*0.5) && this.z - 0.05 < blocks[b].z + (blocks[b].dz*0.5) && this.z + 0.05 > blocks[b].z - (blocks[b].dz*0.5)){
                    inside = true;
                }

                if(inside){
                
                    if(blocks[b].y + (blocks[b].dy/2) <= this.y){
                        highestPoint = Math.max(blocks[b].y + (blocks[b].dy/2) + 0.05, highestPoint);
                    }
                
                }
            }

            for(let c=0; c<craftingTables.length; c++){
                let inside;
        
                if(this.x - 0.05 < craftingTables[c].rect.x + (craftingTables[c].rect.dx*0.5) && this.x + 0.05 > craftingTables[c].rect.x - (craftingTables[c].rect.dx*0.5) && this.z - 0.05 < craftingTables[c].rect.z + (craftingTables[c].rect.dz*0.5) && this.z + 0.05 > craftingTables[c].rect.z - (craftingTables[c].rect.dz*0.5)){
                    inside = true;
                }

                if(inside){
                
                    if(craftingTables[c].rect.y + (craftingTables[c].rect.dy/2) <= this.y){
                        highestPoint = Math.max(craftingTables[c].rect.y + (craftingTables[c].rect.dy/2) + 0.05, highestPoint);
                    }
                
                }
            }

            this.y = highestPoint;
            this.onGround = true;

        }

        this.model.position.x = this.x;
        this.model.position.y = this.y;
        this.model.position.z = this.z;
    }
}