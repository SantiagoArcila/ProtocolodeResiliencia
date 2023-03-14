defmodule GraphUtilsTest do
  use ExUnit.Case

  alias GraphUtils
  # doctest ResilenceProtocol
  test "given the graph creates the png of it" do
    flunk("to implement")
  end

  test "given the graph returns the dotfile conversion" do
    graph = Graph.new() |> Graph.add_vertices([:a, :b, :c, :d])
    assert :ok = GraphUtils.to_dot_file(graph)
    flunk("assert file extension is .dot")
    flunk("remove the file once the test is terminated")
  end

  test "given the dot file generates the png image of the graph representation" do
    flunk("to implement")
  end
end
