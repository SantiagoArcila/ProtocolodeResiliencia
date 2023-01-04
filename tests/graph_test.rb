require_relative '../lib/graph.rb'
require 'test/unit'
 
class TestGraph < Test::Unit::TestCase

  def test_add_node_to_graph
    graph = Graph.new
    node = Node.new(100)
    graph.add_node(node)
    assert_equal(graph.data[node.object_id], node)
  end

  def test_add_edge
    graph = Graph.new
    node_1 = Node.new(100)
    node_2 = Node.new(200)
    graph.add_node(node_1)
    graph.add_node(node_2)

    #from --> to
    graph.add_edge(node_1, node_2)
    node_1.outgoing_edge_to_node == node_2.object_id
    node_2.incoming_edge_from_node == node_1.object_id
  end

end
