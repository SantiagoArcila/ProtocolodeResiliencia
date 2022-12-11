require_relative '../lib/node.rb'
require 'test/unit'
 
class TestNode < Test::Unit::TestCase
 
  def test_node_creation 
    node = Node.new(0,100)
    assert_equal(node.id,0)
    assert_equal(node.balance,100)
  end

  def test_node_modification
    node = Node.new(0,100)
    assert_equal(node.id,0)
    assert_equal(node.balance,100)
    assert_raise "undefined method" do
      node.id = 2
    end
    assert_equal(node.balance = 200, 200)
  end
end
