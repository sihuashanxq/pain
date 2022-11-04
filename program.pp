class program{
    fn main(){
        let a=new ddd(2)
        return a.say()
    }
}

class abc{
    fn ctor(age){
        this.age=age
    }

    fn test(){
        return this.age+this.bbb()
    }
}

class ddd extends abc{
    fn ctor(age){
        super(age)
    }

    fn say(){
        return this.test()
    }

    fn bbb(){
        return 2
    }
}