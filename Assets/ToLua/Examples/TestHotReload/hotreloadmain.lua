local br = require("bellringer")
require('Gbellringer')
local gettime = br.gettime

testhotreload.onPrintTime = testhotreload.onPrintTime + gettime

function printtime()
    br:getname()
    print("print time by br.gettime()")
    br.gettime()
    print("print time by gettime()")
    gettime()

    print("print by Gbellringer")
    Gbellringer:gettime()
end