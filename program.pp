import { Console } from "@runtime.types"
class Program {
    fn main(){
       let p=new Program()
       try{
        p.tryCatch();
       }catch e{
            Console.log("main"+e)
       }
       try{
         p.tryCatchThrow();
       }catch e{
            Console.log("main "+e)
       }
       try{
         Console.log(p.tryCatchReturn());
       }catch e{
            Console.log("main"+e)
       }
    }
    fn tryCatch(){
        try{
            throw "try Catch"
        }catch e{
            Console.log(e)
        }
        finally{
            Console.log("finally try Catch")
        }
    }
    fn tryCatchThrow(){
        try{
            throw "try Catch Throw"
        }catch e{
            Console.log(e)
            throw "rethrow"
        }
        finally{
            Console.log("finally try Catch Throw")
        }
    }
    fn tryCatchReturn(){
        try{
            return "try Catch Return"
        }catch e{
            Console.log(e)
        }
        finally{
            Console.log("finally try Catch Return")
        }
    }
}
