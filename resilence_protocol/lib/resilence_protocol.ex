defmodule ResilenceProtocol do
  @moduledoc """
  initial effor to simulate `ResilenceProtocol` simulation.
  """

  # hold any necesary info that the graph (nodes and edges) will need
  @node_table Module.concat(__MODULE__, "node_properties")
  @edge_table Module.concat(__MODULE__, "edge_properties")

  @spec initialice_protocol() :: Graph.t()
  def initialice_protocol() do
    create_ets_tables()
    Graph.new()
  end

  @doc """
  creates the node if does not exists
  """
  @spec set_balance(Graph.t(), term(), number()) :: Graph.t()
  def set_balance(graph, node, value) do
    dbg()
    :ets.insert(@node_table, {node, %{balance: value}})
    Graph.add_vertex(graph, node)
  end

  @spec get_node_variables(term()) :: list()
  def get_node_variables(node) do
    dbg(node)
    :ets.lookup(@node_table, node)
  end

  @spec print_tables_info() :: :ok
  def print_tables_info() do
    :ets.i(@node_table)
    :ets.i(@edge_table)
  end

  @spec create_ets_tables() :: :ok
  defp create_ets_tables() do
    :ets.new(@node_table, [:named_table])
    :ets.new(@edge_table, [:named_table])
  end
end
