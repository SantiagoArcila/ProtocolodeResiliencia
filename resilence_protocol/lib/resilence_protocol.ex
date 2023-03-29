defmodule ResilenceProtocol do
  @moduledoc """
  initial effor to simulate `ResilenceProtocol` simulation.
  """

  # hold any necesary info that the graph (vertexs and edges) will need
  @vertex_table Module.concat(__MODULE__, "vertex_properties")
  @edge_table Module.concat(__MODULE__, "edge_properties")

  @spec initialice_protocol() :: Graph.t()
  def initialice_protocol() do
    create_ets_tables()
    Graph.new()
  end

  @doc """
  creates the vertex if does not exists
  """
  # TODO: create a generic method to set any key matching from option
  @spec set_balance(Graph.t(), term(), number()) :: Graph.t()
  def set_balance(graph, vertex, value) do
    :ets.insert(@vertex_table, {vertex, %{balance: value}})
    Graph.add_vertex(graph, vertex)
  end

  @doc """
  get the vertex variables
  """
  @spec get_vertex_variables(term()) :: list()
  def get_vertex_variables(vertex) do
    :ets.lookup(@vertex_table, vertex)
  end

  @doc """
  debug helper to print the variables stored in vertex and edges
  """
  @spec print_tables_info() :: :ok
  def print_tables_info() do
    :ets.i(@vertex_table)
    :ets.i(@edge_table)
  end

  @spec create_ets_tables() :: :ok
  defp create_ets_tables() do
    :ets.new(@vertex_table, [:named_table])
    :ets.new(@edge_table, [:named_table])
  end
end
