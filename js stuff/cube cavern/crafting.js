class CraftingTable{
    constructor(x,y,z){
        this.x = x;
        this.y = y;
        this.z = z;

        this.rect = new Rect(this.x, this.y-0.4, this.z, 0.7, 0.2, 0.7, "log", 0.7);

        this.visual = new THREE.Mesh(new THREE.BoxGeometry(0.6, 0.05, 0.6), new THREE.MeshBasicMaterial( { color: 0xeb7734, side: THREE.DoubleSide, transparent: true, opacity: 0.6 } ));
        
        this.visual.position.x = this.x;
        this.visual.position.y = this.y-0.3;
        this.visual.position.z = this.z;
        
        scene.add(this.visual);
        
    }

    check(){
        let itemsInside = {
            indexes: []
        };
        let itemCount = 0;
        for(let i=0; i<items.length; i++){
            if(items[i].x > this.x-0.3 && items[i].x < this.x + 0.3 && items[i].z > this.z-0.3 && items[i].z < this.z + 0.3 && items[i].y > this.y-0.3 && items[i].y < this.y){
                itemsInside[items[i].item] = true;
                itemsInside.indexes.push(i);
                itemCount++;
            }
        }

        if(itemsInside.Stick && itemsInside.Rock && itemCount === 2){
            itemsRemove(itemsInside.indexes);
            items.push(new Item("Dagger", this.x, this.y, this.z));
        }
    }

    collideFloor(){
        this.rect.collideFloor();
    }

    collide(){
        this.rect.collide();
    }

    unload(){
        this.rect.unload();
        scene.remove(this.visual);
        this.visual.geometry.dispose();
        if (this.visual.material instanceof THREE.Material) {
            this.visual.material.dispose();
        } else if (Array.isArray(this.visual.material)) {
            this.visual.material.forEach(material => material.dispose());
        }
        delete this.visual;
    }
}