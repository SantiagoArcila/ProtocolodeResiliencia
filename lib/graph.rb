require_relative './node.rb'

class Graph
  # attr_accessor :data

  def initialize
    @data = {}
  end

  def add_node(node)
    @data.store(node.object_id,node)
  end

  def add_edge(outgoing_edge_node, incoming_edge_node)
    if not (@data.has_key?(outgoing_edge_node.object_id))
      raise "outgoing_node is not in the graph"
    end

    if not (@data.has_key?(incoming_edge_node.object_id))
      raise "outgoing_node is not in the graph"
    end


  end

  def get_node(node)
    @data[node]
  end

end
