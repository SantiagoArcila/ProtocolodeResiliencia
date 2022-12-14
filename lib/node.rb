class Node
  attr_accessor :balance, :incoming_edge, :outgoing_edge

  def initialize(balance)
    @balance = balance
    @incoming_edge = nil
    @outgoing_edge = nil
  end
end
