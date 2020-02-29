local br = require("bellringer")
local gettime = br.gettime

testhotreload.onPrintTime = testhotreload.onPrintTime + gettime

function printtime()
    br:getname()
    print("print time by br.gettime()")
    br.gettime()
    print("print time by gettime()")
    gettime()
end