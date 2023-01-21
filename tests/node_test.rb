require_relative '../lib/node.rb'
require 'test/unit'
 
class TestNode < Test::Unit::TestCase
 
  def test_node_creation 
    node = Node.new(100)
    assert_equal(node.balance,100)
    assert_equal(node.incoming_edge_from_node, nil)
    assert_equal(node.outgoing_edge_to_node, nil)
  end

  def test_node_modification
    node = Node.new(100)
    assert_equal(node.balance,100)
    
    node.incoming_edge_from_node = Node.new(100)
    node.outgoing_edge_to_node = Node.new(200)
    assert_equal(node.balance = 200, 200)
    assert node.incoming_edge_from_node
    assert node.outgoing_edge_to_node
  end
end
