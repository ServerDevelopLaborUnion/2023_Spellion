import MySQL, {RowDataPacket} from 'mysql2/promise'
//async, await 를 쓰게 해주는 녀석 promise

const poolOption : MySQL.PoolOptions = 
{
    host:'gondr99.iptime.org',
    user:'yy_20113',
    password:'1234',
    database:'yy_20113',
    connectionLimit:10
}

export interface UserVO extends RowDataPacket
{
    id: number,
    name: string,
    level: number,
    money: number,
}

export const Pool : MySQL.Pool = MySQL.createPool(poolOption);
