require_relative './node.rb'

class Graph
  attr_accessor :data

  def initialize
    @data = {}
  end

  def add_node(node)
    @data.store(node.object_id,node)
  end

  #from ,to 
  def add_edge(outgoing_edge_node, incoming_edge_node)

    outgoing_edge_node_id = outgoing_edge_node.object_id
    incoming_edge_node_id = incoming_edge_node.object_id

    if not (@data.has_key?(outgoing_edge_node_id))
      raise "outgoing_node is not in the graph"
    end

    if not (@data.has_key?(incoming_edge_node_id))
      raise "incoming_node is not in the graph"
    end

    if outgoing_edge_node_id == incoming_edge_node_id
      raise "adding a node to itself is redundant and could potentially create a cycle"
    end

   @data[outgoing_edge_node_id].outgoing_edge_to_node = incoming_edge_node_id 
   @data[incoming_edge_node_id].incoming_edge_from_node = outgoing_edge_node_id

  end

  def get_node(node)
    @data[node]
  end

end
