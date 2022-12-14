require_relative '../lib/graph.rb'
require 'test/unit'
 
class TestGraph < Test::Unit::TestCase
 
  def test_graph_creation 
    assert false
  end

  def test_add_node_to_graph
    graph = Graph.new
    node = Node.new(0,100)
    graph.add_node(node)
    assert_equal(graph[node], node)
  end

end
