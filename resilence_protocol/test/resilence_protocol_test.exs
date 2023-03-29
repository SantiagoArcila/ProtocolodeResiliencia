defmodule ResilenceProtocolTest do
  use ExUnit.Case

  setup do
    %{graph: ResilenceProtocol.initialice_protocol()}
  end

  test "creates a graph with balance for each vertex", %{graph: graph} do
    vertices = [:a, :b, :c]

    _result_graph =
      Enum.reduce(vertices, graph, fn vertex, acc ->
        ResilenceProtocol.set_balance(acc, vertex, 1)
      end)

    for vertex <- vertices do
      assert [{^vertex, %{balance: 1}}] = ResilenceProtocol.get_vertex_variables(vertex)
    end
  end
end
