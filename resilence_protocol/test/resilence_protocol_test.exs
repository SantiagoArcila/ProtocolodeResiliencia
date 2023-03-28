defmodule ResilenceProtocolTest do
  use ExUnit.Case

  setup do
    %{graph: ResilenceProtocol.initialice_protocol()}
  end

  test "creates a graph with balance for each node", %{graph: graph} do
    result_graph =
      Enum.reduce([?a..?c], graph, fn node, graph ->
        ResilenceProtocol.set_balance(graph, node, 1)
      end)

    assert ResilenceProtocol.get_node_variables(?a) == 1

    dbg(result_graph)
    graph = flunk("to implement")
  end
end
