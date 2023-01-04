class Node
  attr_accessor :balance, :incoming_edge_from_node, :outgoing_edge_to_node

  def initialize(balance)
    @balance = balance
    @incoming_edge_from_node = nil
    @outgoing_edge_to_node = nil
  end
end
