import { Console } from "@runtime.types"
class Program {
    fn main(){
        let p=new Program()
        Console.log(p.sum(1000,new Object()))
    }

    fn sum(n,cache){
        if cache[n]{
            return cache[n]
        }

        if n>=2 {
            cache[n-1]=this.sum(n-1,cache)
            cache[n-2]=this.sum(n-2,cache)
            return cache[n-1]+cache[n-2]
        }

        return n
    }
}
