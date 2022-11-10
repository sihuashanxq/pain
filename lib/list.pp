
class List{
    fn ctor(){
       this.head=null
       this.tail=null
    }

    fn addFront(value){
        let node=new Node(value)
        node.next=this.head
        this.head=node

        if this.tail==null {
            this.tail=node
        }
    }

    fn addBack(value){
        let node=new Node(value)
        this.tail.next=node
        this.tail=node
    }

    fn toArray(){
        let i=0
        let array=new Array()

        for let node= this.head; node!= null; node= node.next {
            array[i]=node.value
            i=i+1
        }

        return array
    }
}

class Node{
    fn ctor(value){
        this.value=value
        this.next=null
    }

    fn toString(){
        return this.value.toString()
    }
}