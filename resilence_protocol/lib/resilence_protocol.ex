defmodule ResilenceProtocol do
  @moduledoc """
  initial effor to simulate `ResilenceProtocol` behaviour.
  """

  @doc """
  creates a random tree with given the quantity of nodes
  """
  @spec create_random_tree(integer()) :: :gb_trees
  def create_random_tree(nodes) do
    Enum.reduce(1..nodes, :gb_trees.empty(), fn node, tree ->
      value = :rand.uniform(100)
      :gb_trees.insert(node, value, tree)
    end)
  end
end
