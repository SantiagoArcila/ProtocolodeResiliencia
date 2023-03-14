defmodule GraphUtils do
  @moduledoc """
  utility functions to work with the graph
  """
  @priv_dir "#{:code.priv_dir(:resilence_protocol)}"

  @lib_dir @priv_dir <> "/lib"
  @graphs_dir @lib_dir <> "/graphs/"
  # will not work in windows
  @dot_binary "/usr/bin/dot"
  @graph_images_dir @lib_dir <> "/graphs/images/"

  @doc """
  Given the graph and the file, will convert it to
  dot file information

  ## Examples
    iex>
  """
  @spec to_dot_file(Graph.t()) :: :ok
  def to_dot_file(graph, graph_file \\ "temp.dot") do
    graphs_dir = @graphs_dir

    {:ok, dot_info} = Graph.to_dot(graph)

    File.write!(graphs_dir <> graph_file, dot_info)
  end

  @spec to_png(atom()) :: {Collectable.t(), non_neg_integer}
  def to_png(binary \\ :dot) do
    binary =
      case binary do
        :dot -> @dot_binary
      end

    dot_file = @graphs_dir <> "temp.dot"
    png_file = @graph_images_dir <> Path.basename(dot_file, ".dot") <> ".png"

    System.cmd(binary, ["-T", "png", dot_file, "-o", png_file])
  end
end
