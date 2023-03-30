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
  make a transaction from vertex source to destination
  it works with the described rule in the paper
  """
  # TODO: refactor ideas, create a helper that bring only the required value instead
  # of pattern matching everywhere

  # TODO: add the helpers to get the exact keys of the ets tables and not pattern match
  # everywhere
  # TODO: add the edge information on the ets table
  # @spec make_transaction(graph, source, destination, value)
  def make_transaction(_graph, source, destination, value) do
    [{_source_vertex, %{balance: source_balance} = source_vertex_variables}] =
      get_vertex_variables(source)

    if source_balance >= value do
      [{_destination_vertex, %{} = destination_vertex_variables}] =
        get_vertex_variables(destination)

      new_source_vertex_variables = Map.update!(source_vertex_variables, :balance, &(&1 - value))

      new_destination_vertex_variables =
        Map.update!(destination_vertex_variables, :balance, &(&1 + value))

      :ets.insert(@vertex_table, {source, new_source_vertex_variables})
      :ets.insert(@vertex_table, {destination, new_destination_vertex_variables})
    else
      raise "Can not do the transaction, insuficient founds"
    end
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
