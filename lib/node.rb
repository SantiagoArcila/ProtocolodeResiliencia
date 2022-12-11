class Node
  attr_reader :id
  attr_accessor :balance

  def initialize(id, balance)
    @id = id
    @balance = balance
  end
end
