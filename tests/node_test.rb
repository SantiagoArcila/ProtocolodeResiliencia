require_relative '../lib/node.rb'
require 'test/unit'
 
class TestNode < Test::Unit::TestCase
 
  def test_node_creation 
    node = Node.new(100)
    assert_equal(node.balance,100)
    assert_equal(node.incoming_edge, nil)
    assert_equal(node.outgoing_edge, nil)
  end

  def test_node_modification
    node = Node.new(100)
    assert_equal(node.balance,100)
    assert_equal(node.incoming_edge, nil)
    assert_equal(node.outgoing_edge, nil)
    assert_raise "undefined method" do
      node.id = 2
    end
    node.incoming_edge = Node.new(100)
    node.outgoing_edge = Node.new(100)
    assert_equal(node.balance = 200, 200)
    assert node.incoming_edge
    assert node.outgoing_edge
  end
end
