defmodule ResilenceProtocolTest do
  use ExUnit.Case

  test "creates a graph with balance for each vertex" do
    graph = ResilenceProtocol.initialice_protocol()
    vertices = [:a, :b, :c]

    _result_graph =
      Enum.reduce(vertices, graph, fn vertex, acc ->
        ResilenceProtocol.set_balance(acc, vertex, 1)
      end)

    for vertex <- vertices do
      assert [{^vertex, %{balance: 1}}] = ResilenceProtocol.get_vertex_variables(vertex)
    end
  end

  test "send transaction with sufficient founds" do
    graph =
      ResilenceProtocol.initialice_protocol()
      |> ResilenceProtocol.set_balance(:a, 3)
      |> ResilenceProtocol.set_balance(:b, 1)

    ResilenceProtocol.make_transaction(graph, :a, :b, 2)
    assert [{:a, %{balance: 1}}] = ResilenceProtocol.get_vertex_variables(:a)
    assert [{:b, %{balance: 3}}] = ResilenceProtocol.get_vertex_variables(:b)
    flunk("check the graph has the edge")
    flunk("check the graph has the edge information")
    flunk("check the tables has the necesary info")
    flunk("to implement")
  end
end
