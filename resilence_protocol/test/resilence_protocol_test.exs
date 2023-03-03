defmodule ResilenceProtocolTest do
  use ExUnit.Case
  doctest ResilenceProtocol

  test "creates a random tree given the num of nodes" do
    tree = ResilenceProtocol.create_random_tree(10)
    assert :gb_trees.size(tree) == 10
  end
end
