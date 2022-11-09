import { Console } from "@runtime.types"
class Program {
    fn main(){
        try{
            throw "Exception throwd"
        }catch e{
            Console.log(e)
            return 1
        }
        finally{
            Console.log("finally")
        }

        try {
            throw "Exception 2"
        }catch e{
            Console.log(e)
            throw e
        }
        finally{
            Console.log("finally2")
        }
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
