class Object {
    fn  __is__(type){
        return this.getType()==type
    }

    fn native __mul__(v)

    fn native __add__(v)

    fn native __sub__(v)

    fn native __div__(v)

    fn native __mod__(v)

    fn native __equal__(v)
}

class Console extends Object {
    
}

class String extends Object{
    fn native len()

    fn native getChar(i)

    fn substring(start,count){
        let str='',len= this.len(),end= start+count
        if start>= len{
            return str
        }

        for let i= start; i< end && i< len; i= i+ 1{
            str += this.getChar(i)
        }

        return str
    }
}