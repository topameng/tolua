
local bellringer = {name='best bellringer'}

function bellringer.gettime()
    print('I dont know current time.')
end

function bellringer:getname()
    print('My name is ' .. self.name)
end

return bellringer
