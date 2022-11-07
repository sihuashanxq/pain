import {List} from "lib.list"
class Program{
    fn main(){
       if true or false{
           Console.log(1)
       }

       if false || true{
           Console.log(2)
       }

       if true&&true{
           Console.log(3)
       }

       if false&&true{
           Console.log(4)
       }

       if true&&false{
           Console.log(5)
       }

       if true&&Program{
           Console.log(6)
       }
    }
}