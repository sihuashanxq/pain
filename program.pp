class program{
    fn main(){
        let x=1
        let b=new ddd()
        let z= fn(d)=>x+d
        let y= b.test(z)
        Console.log("Value is :"+y)
    }
}

class ddd {
    fn say(){
        return this.test()
    }

    fn test(f){
        return f(2)
    }
}