
import {List} from "lib.list"
class Program{
    fn main(){
        let list=new List()
        list.addFront(1)
        list.addFront(2)
        list.addFront(3)
        Console.log(list.toArray().toString())
    }
}